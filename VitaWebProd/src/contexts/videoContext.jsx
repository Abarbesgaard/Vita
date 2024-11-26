import { createContext, useReducer, useEffect, useState } from "react";
import { VideoReducer } from "../reducers/VideoReducer";
import {
	getAllVideosFake as getAll,
	saveVideoFake as save,
	updateVideoFake as update,
	deleteVideoFromDbFake as destroy,
} from "../APIs/VideoAPI";
import { getSessionToken } from "../services/supabase";

const VideoContext = createContext(null);

const VideoProvider = ({ children }) => {
	const [videos, dispatch] = useReducer(VideoReducer, []);

	useEffect(() => {}, []);

	const fetchVideos = async () => {
		const token = await getSessionToken();
		const data = await getAll(token);
		if (data.videos) {
			dispatch({ type: "getAllVideos", payload: data.videos });
		}
		console.log("fetchVideos");
	};

	const saveVideo = async (video) => {
		const token = await getSessionToken();
		const data = await save(video, token);
		if (!data.error) {
			dispatch({ type: "addVideo", payload: data.video });
		}
	};

	const updateVideo = async (video) => {
		const token = await getSessionToken();
		const data = await update(video, token);
		if (!data.error) {
			dispatch({ type: "updateVideo", payload: video });
		}
	};

	const deleteVideo = async (id) => {
		const token = await getSessionToken();
		const data = await destroy(id, token);
		if (!data.error) {
			dispatch({ type: "deleteVideo", payload: id });
		}
	};

	return (
		<VideoContext.Provider
			value={{ videos, saveVideo, updateVideo, deleteVideo, fetchVideos }}
		>
			{children}
		</VideoContext.Provider>
	);
};

export { VideoProvider, VideoContext };
