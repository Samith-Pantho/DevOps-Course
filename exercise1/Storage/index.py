from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from Routes.StorageRoutes import StorageRoutes

app = FastAPI(
    title="Storage API",
    description="Storage",
    version="1.0.0"
)

# Include all routers
app.include_router(StorageRoutes)


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