import { useState } from "react";
import VideoCard from "../components/VideoCard";
import VideoForm from "../components/VideoForm";
import Layout from "../components/Layout";

export default function Dashboard() {
	const [title, setTitle] = useState("");
	const [linkUrl, setLinkUrl] = useState("");
	const [videos, setVideos] = useState([
		{
			id: 1,
			title: "Dirt Man",
			url: "https://www.youtube.com/embed/VWXzklIphhU",
		},
		{
			id: 2,
			title: "Internet Drama",
			url: "https://www.youtube.com/embed/zr1h2Z11oTI",
		},
	]);
	const deleteVideo = (id) => {
		const isConfirmed = confirm("Er du sikker?");

		if (isConfirmed) {
			const removed = videos.filter((video) => video.id !== id);
			setVideos(removed);
		}
	};

	const handleVideoFormSubmit = (e) => {
		e.preventDefault();

		setVideos([...videos, { id: videos.length + 1, title, url: linkUrl }]);
	};

	return (
		<Layout>
			<div className="bg-slate-400 w-full h-full flex flex-col items-center overflow-auto">
				<VideoForm
					handleVideoFormSubmit={handleVideoFormSubmit}
					title={title}
					setTitle={setTitle}
					url={linkUrl}
					setUrl={setLinkUrl}
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
