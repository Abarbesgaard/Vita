import { useEffect, useState } from "react";
import VideoCard from "../components/Video/VideoCard";
import VideoForm from "../components/Video/VideoForm";
import Layout from "../components/Layout";
import { saveVideo, getAllVideos, deleteVideoFromDb } from "../API/VideoAPI";
import { useAuth0 } from "@auth0/auth0-react";

export default function Dashboard() {
	const { user } = useAuth0();
	const [title, setTitle] = useState("");
	const [description, setDescription] = useState("");
	const [linkUrl, setLinkUrl] = useState("");
	const [videos, setVideos] = useState([]);

	const deleteVideo = async (id) => {
		const isConfirmed = window.confirm(
			"Er du sikker på at du vil slette videoen?"
		);

		if (isConfirmed) {
			await deleteVideoFromDb(id);
			console.log("delete");
			await fetchVideos();
		}
	};

	const handleVideoFormSubmit = async (e) => {
		e.preventDefault();
		const video = await saveVideo(
			{
				title,
				url: linkUrl.replace("youtube", "youtube-nocookie"),
				description,
			},
			user.sub
		);
		setDescription("");
		setTitle("");
		setLinkUrl("");
		await fetchVideos();
	};

	const fetchVideos = async () => {
		const videos = await getAllVideos(user.user_id);
		console.log(videos);
		setVideos([...videos]);
		console.log(user);
	};

	useEffect(() => {
		fetchVideos();
	}, []);

	return (
		<Layout>
			<div className="bg-slate-400 w-full h-full flex flex-col lg:flex-row overflow-auto">
				<VideoForm
					handleVideoFormSubmit={handleVideoFormSubmit}
					title={title}
					setTitle={setTitle}
					url={linkUrl}
					setUrl={setLinkUrl}
					description={description}
					setDescription={setDescription}
				/>
				<div className="flex flex-col w-full">
					{videos.map((video) => (
						<VideoCard key={video.id} video={video} deleteVideo={deleteVideo} />
					))}
				</div>
			</div>
		</Layout>
	);
}
