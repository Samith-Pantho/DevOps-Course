
import os

async def SaveLog(message: str):
    try:
        log_message = (
            f"{message}\n"
        )

        # Build path
        log_dir = "/app/vStorage"
        os.makedirs(log_dir, exist_ok=True)

        log_filename = "vStorage.txt"
        log_path = os.path.join(log_dir, log_filename)

        # Append log message
        with open(log_path, "a", encoding="utf-8") as f:
            f.write(log_message)
    except Exception as ex:
        print(f"Logging failed: {ex}")