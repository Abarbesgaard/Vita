import { createContext, useReducer, useEffect, useState } from "react";
import { VideoReducer } from "../reducers/VideoReducer";
import {
	getAllVideosFake as getAll,
	saveVideoFake as save,
	updateVideoFake as update,
	deleteVideoFromDbFake as destroy,
} from "../APIs/VideoAPI";

const VideoContext = createContext(null);

const VideoProvider = ({ children }) => {
	const [videos, dispatch] = useReducer(VideoReducer, []);

	useEffect(() => {
		const fetchVideos = async () => {
			const data = await getAll();
			if (data.videos) {
				dispatch({ type: "getAllVideos", payload: data.videos });
			}
		};
		fetchVideos();
	}, []);

	const saveVideo = async (video) => {
		const data = await save(video);
		if (!data.error) {
			dispatch({ type: "addVideo", payload: data.video });
		}
	};

	const updateVideo = async (video) => {
		const data = await update(video);
		if (!data.error) {
			dispatch({ type: "updateVideo", payload: data.video });
		}
	};

	const deleteVideo = async (id) => {
		const data = await destroy(id);
		console.log("deleteVideo", data);
		if (!data.error) {
			dispatch({ type: "deleteVideo", payload: id });
		}
	};

	return (
		<VideoContext.Provider
			value={{ videos, saveVideo, updateVideo, deleteVideo }}
		>
			{children}
		</VideoContext.Provider>
	);
};

export { VideoProvider, VideoContext };
