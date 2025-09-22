
import os

async def SaveLog(message: str):
    try:
        log_message = (
            f"{message}\n"
        )

        # Build path
        log_dir = "/app"

        log_filename = "vStorage"
        log_path = os.path.join(log_dir, log_filename)

        # Append log message
        with open(log_path, "a", encoding="utf-8") as f:
            f.write(log_message)
    except Exception as ex:
        print(f"Logging failed: {ex}")