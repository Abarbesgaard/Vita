export const VideoReducer = (state, action) => {
	console.log("Reducer action:", action.type, "Payload:", action.payload);

	switch (action.type) {
		case "getAllVideos":
			return action.payload;
		case "addVideo":
			return [...state, action.payload];
		case "updateVideo":
			return state.map((video) =>
				video.id === action.payload.id ? action.payload : video
			);
		case "deleteVideo":
			return state.filter((video) => video.id !== action.payload);
		default:
			return state;
	}
};
