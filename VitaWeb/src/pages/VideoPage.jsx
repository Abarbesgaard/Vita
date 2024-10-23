import { useEffect, useState } from "react";
import Layout from "../components/Layout";
import {
	saveVideo,
	getAllVideos,
	deleteVideoFromDb,
	updateVideo,
} from "../API/VideoAPI";
import { useAuth } from "../context/AuthContext";
import { Navigate } from "react-router-dom";
import { AiOutlineVideoCameraAdd } from "react-icons/ai";
import { getSessionToken } from "../services/Supabase";
import EmptyVideo from "../components/Video/EmptyVideo";
import AddVideoModal from "../components/Video/AddVideoModal";
import VideoTable from "../components/Video/VideoTable";
import VideoTableSkeleton from "../components/Video/VideoTableSkeleton";

export default function VideoPage() {
	const { user } = useAuth();
	const [id, setId] = useState("");
	const [title, setTitle] = useState("");
	const [description, setDescription] = useState("");
	const [linkUrl, setLinkUrl] = useState("");
	const [video, setVideo] = useState(null);
	const [videos, setVideos] = useState(null);
	const [token, setToken] = useState("");
	const [isLoading, setIsLoading] = useState(true);
	const [showAddVideoModal, setShowAddVideoModal] = useState(false);
	const [editMode, setEditMode] = useState("false");

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
			await saveVideo(
				{
					title,
					url: linkUrl,
					description,
				},
				token
			);
		}
		if (editMode) {
			await updateVideo({ id, title, url: linkUrl, description }, token);
		}
		setId("");
		setDescription("");
		setTitle("");
		setLinkUrl("");
		await fetchVideos();
		setShowAddVideoModal(false);
		setEditMode(false);
	};

	const handleEdit = async (id, title, url, description) => {
		setEditMode(true);
		setId(id);
		setTitle(title);
		setLinkUrl(url);
		setDescription(description);
		setShowAddVideoModal(true);
	};

	const fetchVideos = async () => {
		const token = await getSessionToken();
		setToken(token);
		const videos = await getAllVideos(token);
		setVideos([...videos]);
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
		<Layout>
			{showAddVideoModal && (
				<AddVideoModal
					setShowAddVideoModal={setShowAddVideoModal}
					handleVideoFormSubmit={handleVideoFormSubmit}
					title={title}
					setTitle={setTitle}
					url={linkUrl}
					setUrl={setLinkUrl}
					description={description}
					setDescription={setDescription}
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
					) : videos.lenght === 0 ? (
						<EmptyVideo handleClick={setShowAddVideoModal} />
					) : (
						<div className="w-full h-full flex flex-col justify-evenly">
							<VideoTable
								videos={videos}
								deleteVideo={deleteVideo}
								handleEdit={handleEdit}
							/>
							<div
								className="w-1/6 h-20 flex flex-col items-center justify-center border-2 border-dashed border-slate-400 hover:border-black cursor-pointer rounded-xl mx-auto mt-10"
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
		</Layout>
	);
}
