from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from Routes.Service2Routes import Service2Routes

app = FastAPI(
    title="Service2 API",
    description="Service2",
    version="1.0.0"
)

# Include all routers
app.include_router(Service2Routes)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],               # leave empty
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/health")
async def health_check():
    return {
        "status": "API is running"
    }