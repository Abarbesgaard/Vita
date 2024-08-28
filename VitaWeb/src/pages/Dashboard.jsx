import { useState } from "react";
import LogoutButton from "../components/LogoutButton";
import UserCard from "../components/UserCard";
import VideoCard from "../components/VideoCard";
import VideoForm from "../components/VideoForm";

export default function Dashboard({ user }) {
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

	const handleVideoFormSubmit = (e) => {
		e.preventDefault();

		setVideos([...videos, { id: videos.length + 1, title, url: linkUrl }]);
	};

	return (
		<div className="h-dvh flex flex-col lg:flex-row bg-gray-300">
			<div className="flex flex-col items-center justify-center px-10 mb-2 lg:mb-0">
				<UserCard user={user} />
				<LogoutButton />
			</div>
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
					/>
				))}
			</div>
		</div>
	);
}
