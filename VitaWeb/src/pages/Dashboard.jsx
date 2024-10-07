import { useEffect, useState } from "react";
import VideoForm from "../components/Video/VideoForm";
import Layout from "../components/Layout";
import { saveVideo, getAllVideos, deleteVideoFromDb } from "../API/VideoAPI";
import VideoAccordion from "../components/Video/VideoAccordion";

export default function Dashboard() {
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
		await saveVideo(
			{
				title,
				url: linkUrl.replace("youtube", "youtube-nocookie"),
				description,
			},
			123
		);
		setDescription("");
		setTitle("");
		setLinkUrl("");
		await fetchVideos();
	};

	const fetchVideos = async () => {
		const videos = await getAllVideos(123);
		console.log(videos);
		setVideos([...videos]);
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
				<div className="flex flex-col w-full py-14 px-10 overflow-visible">
					<VideoAccordion videos={videos} deleteVideo={deleteVideo} />
				</div>
			</div>
		</Layout>
	);
}
