import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { useVideo } from "../../hooks/useVideo";
import { Navigate } from "react-router-dom";
import { AiOutlineVideoCameraAdd } from "react-icons/ai";
import EmptyVideo from "./components/EmptyVideo";
import AddVideoModal from "./components/AddVideoModal";
import VideoTable from "./components/VideoTable";
import VideoTableSkeleton from "./components/VideoTableSkeleton";

export default function VideoPage() {
	const { videos, fetchVideos } = useVideo();
	const { user } = useAuth();
	const [video, setVideo] = useState({
		title: "",
		url: "",
		description: "",
	});
	const [isLoading, setIsLoading] = useState(true);
	const [showAddVideoModal, setShowAddVideoModal] = useState(false);
	const [editMode, setEditMode] = useState(false);

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

	useEffect(() => {
		const initialFetch = async () => {
			await fetchVideos();
			setIsLoading(false);
		};
		initialFetch();
	}, []);

	if (!user) {
		return <Navigate to="/login" replace />;
	}

	return (
		<>
			{showAddVideoModal && (
				<AddVideoModal
					setShowAddVideoModal={setShowAddVideoModal}
					video={video}
					setVideo={setVideo}
					mode={editMode}
				/>
			)}
			<div className="w-3/4 mx-auto h-full flex flex-col lg:flex-row overflow-auto justify-center lg:items-center">
				<div className="flex flex-col w-full px-10 items-center overflow-visible">
					{isLoading ? (
						<div className="w-full">
							<VideoTableSkeleton />
							<div className="w-1/6 h-20 flex flex-col items-center justify-center rounded-xl mx-auto mt-10 bg-gray-400 animate-pulse"></div>
						</div>
					) : videos.length === 0 ? (
						<EmptyVideo handleClick={setShowAddVideoModal} />
					) : (
						<div className="w-full">
							<VideoTable videos={videos} handleEdit={handleEdit} />
							<div
								className="w-4/6 md:w-2/6 xl:w-1/6 h-20 flex flex-col items-center justify-center border-2 border-dashed border-slate-400 hover:border-black cursor-pointer rounded-xl mx-auto mt-10 transition-all"
								onClick={() => {
									setEditMode(false);
									setVideo({
										title: "",
										url: "",
										description: "",
									});
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
