export const getAllVideos = async (token) => {
	try {
		const response = await fetch("https://localhost:8081/api/video/getall", {
			headers: {
				noCors: true,
				Authorization: "Bearer " + token,
			},
		}).then((response) => response.json());
		console.log(response);
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

export const updateVideo = async (video, token) => {
	try {
		console.log(video);
		const response = await fetch(
			`https://localhost:8081/api/video/put/${video.id}`,
			{
				method: "PUT",
				headers: {
					Authorization: "Bearer " + token,
					"Content-Type": "application/json",
				},
				body: JSON.stringify({
					title: video.title,
					description: video.description,
					url: video.url,
				}),
			}
		).then((response) => response.json());
		console.log(response);
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
