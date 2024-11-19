export const getAllVideos = async (token) => {
	const data = { videos: null, error: null };

	try {
		const response = await fetch("https://localhost:8081/api/video/getall", {
			headers: {
				noCors: true,
				Authorization: "Bearer " + token,
			},
		}).then((response) => response.json());
		data.videos = response;
	} catch (error) {
		data.error = error;
	}
	return data;
};
export const getAllVideosFake = async () => {
	const data = { videos: null, error: null };

	try {
		const response = await fetch("/api/videos", {}).then((response) =>
			response.json()
		);
		data.videos = response.videos;
	} catch (error) {
		data.error = error;
	}
	return data;
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
export const saveVideoFake = async (video) => {
	const data = { video: null, error: null };

	try {
		const response = await fetch("/api/videos", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				charset: "utf-8",
			},
			body: JSON.stringify(video),
		}).then((response) => response.json());

		data.video = response.video;
	} catch (error) {
		data.error = error;
	}
	return data;
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
export const updateVideoFake = async (video) => {
	const data = { video: null, error: null };

	try {
		const response = await fetch(`/api/videos/${video.id}`, {
			method: "PUT",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({
				title: video.title,
				description: video.description,
				url: video.url,
			}),
		}).then((response) => response.json());

		data.video = response.video;
	} catch (error) {
		data.error = error;
	}
	return data;
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
export const deleteVideoFromDbFake = async (id) => {
	const data = { video: null, error: null };
	try {
		const response = await fetch(`/api/videos/${id}`, {
			method: "DELETE",
		});

		data.video = response.video;
	} catch (error) {
		data.error = error;
	}
	return data;
};
