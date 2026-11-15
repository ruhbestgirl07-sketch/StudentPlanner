window.studentPlanner = {
    downloadText: (fileName, mimeType, content) => {
        if (typeof fileName !== "string" || typeof content !== "string") {
            throw new TypeError("A file name and text content are required.");
        }

        const blob = new Blob(["\uFEFF", content], { type: mimeType || "text/plain;charset=utf-8" });
        const objectUrl = URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = objectUrl;
        link.download = fileName;
        link.hidden = true;
        document.body.appendChild(link);

        try {
            link.click();
        } finally {
            link.remove();
            // Delay revocation so browsers have time to begin consuming the generated file.
            window.setTimeout(() => URL.revokeObjectURL(objectUrl), 1000);
        }
    }
};
