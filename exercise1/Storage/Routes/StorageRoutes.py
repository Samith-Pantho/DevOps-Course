import os
import traceback
from fastapi import APIRouter, Request, status
from fastapi.responses import JSONResponse

StorageRoutes = APIRouter()
log_dir = "/app/Storage/logs"

@StorageRoutes.post("/log")
async def SaveLog(request: Request):
    try:
        data = await request.json()
        await _SaveLogAsync(data.get("response"))
        return JSONResponse(
                status_code=status.HTTP_200_OK,
                content={"success": True}
            )
    except Exception as ex:
        error_msg = f"{str(ex)}\n{traceback.format_exc()}"
        return JSONResponse(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            content={"error": error_msg}
        )


@StorageRoutes.get("/log")
async def GetLog():
    try:
        log_path = os.path.join(log_dir, "logs.txt")
        if not os.path.exists(log_path):
            return JSONResponse(
                status_code=status.HTTP_200_OK,
                content={"log": ""}
            )
        
        with open(log_path, "r", encoding="utf-8") as f:
            logList = f.read()
        return JSONResponse(
                status_code=status.HTTP_200_OK,
                content={"log": logList}
            )
    except Exception as ex:
        error_msg = f"{str(ex)}\n{traceback.format_exc()}"
        return JSONResponse(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            content={"error": error_msg}
        )
        
@StorageRoutes.delete("/log")
async def DeleteLog():
    try:
        log_path = os.path.join(log_dir, "logs.txt")
        if os.path.exists(log_path):
            os.remove(log_path)
            
        return JSONResponse(
                status_code=status.HTTP_200_OK,
                content={"success": True}
            )
    except Exception as ex:
        error_msg = f"{str(ex)}\n{traceback.format_exc()}"
        return JSONResponse(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            content={"error": error_msg}
        )        
        
async def _SaveLogAsync(message: str):
    try:
        log_message = (
            f"{message}\n"
        )

        # Build path
        os.makedirs(log_dir, exist_ok=True)

        log_filename = "logs.txt"
        log_path = os.path.join(log_dir, log_filename)

        # Append log message
        with open(log_path, "a", encoding="utf-8") as f:
            f.write(log_message)
    except Exception as ex:
        print(f"Logging failed: {ex}")