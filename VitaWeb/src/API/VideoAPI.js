export const getAllVideos = async (token) => {
	try {
		const response = await fetch("https://localhost:8081/api/video/getall", {
			headers: {
				noCors: true,
				Authorization: "Bearer " + token,
			},
		}).then((response) => response.json());

		return response;
	} catch (error) {
		return error;
	}
};

export const saveVideo = async (video, token) => {
	try {
		const response = await fetch("https://localhost:8081/api/video/create", {
			method: "POST",
			headers: {
				Authorization: "Bearer " + token,
				"Content-Type": "application/json",
				charset: "utf-8",
			},
			body: JSON.stringify(video),
		}).then((response) => response.json());

		return response;
	} catch (error) {
		return error;
	}
};

export const deleteVideoFromDb = async (id, token) => {
	try {
		const response = await fetch(
			`https://localhost:8081/api/video/delete/${id}`,
			{
				method: "DELETE",
				headers: {
					Authorization: "Bearer " + token,
				},
			}
		).then((response) => response.json());

		return response;
	} catch (error) {
		return error;
	}
};
