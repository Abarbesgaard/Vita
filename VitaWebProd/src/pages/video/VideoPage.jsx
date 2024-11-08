import { useEffect, useState } from "react";
import {
	saveVideoFake as saveVideo,
	getAllVideosFake as getAllVideos,
	deleteVideoFromDbFake as deleteVideoFromDb,
	updateVideoFake as updateVideo,
} from "../../APIs/VideoAPI";
import { useAuth } from "../../contexts/useAuth";
import { Navigate } from "react-router-dom";
import { AiOutlineVideoCameraAdd } from "react-icons/ai";
import { getSessionToken } from "../../services/supabase";
import EmptyVideo from "./components/EmptyVideo";
import AddVideoModal from "./components/AddVideoModal";
import VideoTable from "./components/VideoTable";
import VideoTableSkeleton from "./components/VideoTableSkeleton";

export default function VideoPage() {
	const { user } = useAuth();
	const [video, setVideo] = useState({
		title: "",
		url: "",
		description: "",
	});
	const [videos, setVideos] = useState(null);
	const [token, setToken] = useState("");
	const [isLoading, setIsLoading] = useState(true);
	const [showAddVideoModal, setShowAddVideoModal] = useState(false);
	const [editMode, setEditMode] = useState(false);

	const deleteVideo = async (id) => {
		const isConfirmed = window.confirm(
			"Er du sikker på at du vil slette videoen?"
		);

		if (isConfirmed) {
			const token = await getSessionToken();
			await deleteVideoFromDb(id, token);
			console.log("delete");
			await fetchVideos();
		}
	};

	const handleVideoFormSubmit = async () => {
		if (!editMode) {
			await saveVideo(video, token);
		}
		if (editMode) {
			await updateVideo(video, token);
		}
		setVideo({
			id: "",
			title: "",
			url: "",
			description: "",
		});
		await fetchVideos();
		setShowAddVideoModal(false);
		setEditMode(false);
	};

	const handleEdit = async (id, title, url, description) => {
		setEditMode(true);
		setVideo({
			id,
			title,
			url,
			description,
		});
		setShowAddVideoModal(true);
	};

	const fetchVideos = async () => {
		const token = await getSessionToken();
		setToken(token);
		const { videos, error } = await getAllVideos(token);
		if (error) {
			alert("Der skete en fejl da videoer skulle hentes fra databasen");
		}
		if (!error) {
			setVideos(videos);
		}
		setIsLoading(false);
	};

	useEffect(() => {
		setIsLoading(true);
		fetchVideos();
	}, []);

	if (!user) {
		return <Navigate to="/login" replace />;
	}

	return (
		<>
			{showAddVideoModal && (
				<AddVideoModal
					setShowAddVideoModal={setShowAddVideoModal}
					handleVideoFormSubmit={handleVideoFormSubmit}
					video={video}
					setVideo={setVideo}
					mode={editMode}
				/>
			)}
			<div className="w-3/4 p-10 mx-auto h-full flex flex-col lg:flex-row overflow-auto justify-center">
				<div className="flex flex-col w-full px-10 items-center overflow-visible">
					{isLoading ? (
						<div className="w-full">
							<VideoTableSkeleton />
							<div className="w-1/6 h-20 flex flex-col items-center justify-center rounded-xl mx-auto mt-10 bg-gray-400 animate-pulse"></div>
						</div>
					) : videos === null ? (
						<EmptyVideo handleClick={setShowAddVideoModal} />
					) : (
						<div className="w-full h-full flex flex-col justify-evenly">
							<VideoTable
								videos={videos}
								deleteVideo={deleteVideo}
								handleEdit={handleEdit}
							/>
							<div
								className="w-4/6 md:w-2/6 xl:w-1/6 h-20 flex flex-col items-center justify-center border-2 border-dashed border-slate-400 hover:border-black cursor-pointer rounded-xl mx-auto mt-10 transition-all"
								onClick={() => {
									setEditMode(false);
									setShowAddVideoModal(true);
								}}
							>
								<AiOutlineVideoCameraAdd className="text-3xl" />
								<p className="text-slate-500">Tilføj ny film</p>
							</div>
						</div>
					)}
				</div>
			</div>
		</>
	);
}
