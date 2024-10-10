

export const getAllVideos = async () => {
	try {
		const response = await fetch("http://localhost:8080/api/video/getall", {
			headers: {
				"Content-Type": "application/json",
				charset: "utf-8",
			},
		});
		
		if (!response.ok) {
			throw new Error('Network response was not ok');
		}

		const data = await response.json();
		return data;
	} catch (error) {
		console.error("Error fetching videos:", error);
		return null; // Return null or handle the error as needed
	}
};

export const createVideo = async (video) => {
	console.log(JSON.stringify(video));
	try {
		const response = await fetch("http://localhost:8080/api/video/create", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				charset: "utf-8",
			},
			body: JSON.stringify(video),
		});
		
		if (!response.ok) {
			throw new Error('Network response was not ok');
		}

		const data = await response.json();
		return data;
	} catch (error) {
		console.error("Error saving video:", error);
		return null; // Return null or handle the error as needed
	}
};

export const deleteVideo = async (id) => {
    try {
        const response = await fetch(`http://localhost:8080/api/video/delete/${id}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
            },
        });

        if (!response.ok) {
            throw new Error('Failed to delete video');
        }

        // Check if the response has content before parsing
        const text = await response.text();
        if (text) {
            return JSON.parse(text);
        } else {
            // If no content, return a success message
            return { success: true, message: "Video deleted successfully" };
        }
    } catch (error) {
        console.error("Error deleting video:", error);
        return { success: false, error: error.message };
    }
};
