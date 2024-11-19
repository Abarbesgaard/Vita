import { VideoContext } from "../contexts/videoContext";
import { useContext } from "react";

export const useVideo = () => {
	return useContext(VideoContext);
};
