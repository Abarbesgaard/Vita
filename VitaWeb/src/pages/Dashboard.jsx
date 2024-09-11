import { useEffect, useState } from "react";
import VideoCard from "../components/VideoCard";
import VideoForm from "../components/VideoForm";
import Layout from "../components/Layout";
import { saveVideo, getAllVideos, deleteVideoFromDb } from "../API/VideoAPI";
import { useAuth0 } from "@auth0/auth0-react";

export default function Dashboard() {
	const { user } = useAuth0();
	const [title, setTitle] = useState("");
	const [description, setDescription] = useState("");
	const [linkUrl, setLinkUrl] = useState("");
	const [videos, setVideos] = useState([]);
	const deleteVideo = (id) => {
		const isConfirmed = window.confirm(
			"Er du sikker på at du vil slette videoen?"
		);

		if (isConfirmed) {
			const removed = deleteVideoFromDb(id);
			console.log(removed);
			fetchVideos();
		}
	};

	const handleVideoFormSubmit = (e) => {
		e.preventDefault();
		const video = saveVideo(
			{
				title,
				url: linkUrl.replace("youtube", "youtube-nocookie"),
				description,
			},
			user.user_id
		);
		console.log(video);
		fetchVideos();
	};

	const fetchVideos = async () => {
		const videos = await getAllVideos(user.user_id);
		console.log(videos);
		setVideos([...videos]);
	};

	useEffect(() => {
		fetchVideos();
	}, []);

	return (
		<Layout>
			<div className="bg-slate-400 w-full h-full flex flex-col items-center overflow-auto">
				<VideoForm
					handleVideoFormSubmit={handleVideoFormSubmit}
					title={title}
					setTitle={setTitle}
					url={linkUrl}
					setUrl={setLinkUrl}
					description={description}
					setDescription={setDescription}
				/>
				{videos.map((video) => (
					<VideoCard
						key={video.id}
						id={video.id}
						title={video.title}
						url={video.url}
						deleteVideo={deleteVideo}
					/>
				))}
			</div>
		</Layout>
	);
}
