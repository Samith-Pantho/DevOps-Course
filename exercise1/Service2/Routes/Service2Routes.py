import shutil
import traceback
from typing import Any, Dict, Optional
from fastapi import APIRouter, HTTPException, status
from fastapi.responses import JSONResponse
import httpx

from Services.LogServices import SaveLog

Service2Routes = APIRouter()
storage = "http://storage:8444"

@Service2Routes.get("/status")
async def getStatus()-> str:
    try:
        uptime_hours = _GetContainerUptimeHours()
        free_mb = _GetFreeDiskSpaceOnRootInMB()

        response = f"Timestamp2: uptime {uptime_hours:.2f} hours, free disk in root: {free_mb} Mbytes"
        payload = {
            "response": response
        }
        headers = {
            "accept": "application/json",
            "content-type": "application/json"
        }
        
        apistatus = await _ApiCall(
            method="POST",
            url=f"{storage}/log",
            headers=headers,
            payload=payload
        )
        if apistatus.get("success") == True:
            await SaveLog(response)
            return JSONResponse(
                status_code=status.HTTP_200_OK,
                content={"response": response}
            )
    except Exception as ex:
        error_msg = f"{str(ex)}\n{traceback.format_exc()}"
        return JSONResponse(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            content={"error": error_msg}
        )

def _GetContainerUptimeHours() -> float:
    with open("/proc/uptime", "r") as f:
        uptime_seconds = float(f.read().split()[0])
    return uptime_seconds / 3600.0

def _GetFreeDiskSpaceOnRootInMB() -> int:
    usage = shutil.disk_usage("/")
    return usage.free // (1024 * 1024)

async def _ApiCall(
    method: str,
    url: str,
    headers: Optional[Dict[str, str]] = None,
    payload: Optional[Dict[str, Any]] = None,
    params: Optional[Dict[str, Any]] = None,
    timeout: int = 10
) -> Dict[str, Any]:
    async with httpx.AsyncClient() as client:
        try:
            response = await client.request(
                method=method.upper(),
                url=url,
                headers=headers,
                json=payload,
                params=params,
                timeout=timeout
            )
            response.raise_for_status()
            return response.json()
            
        except httpx.HTTPStatusError as e:
            error_msg = f"HTTP error {e.response.status_code}: {str(e)}\n{traceback.format_exc()}"
            raise HTTPException(
                status_code=e.response.status_code,
                detail=f"External API error: {error_msg}"
            )
            
        except httpx.RequestError as e:
            error_msg = f"Request failed: {str(e)}\n{traceback.format_exc()}"
            raise HTTPException(
                status_code=503,
                detail=f"Service unavailable: {error_msg}"
            )
            
        except Exception as e:
            error_msg = f"Unexpected error: {str(e)}\n{traceback.format_exc()}"
            raise HTTPException(
                status_code=500,
                detail=f"Internal server error: {error_msg}"
            )