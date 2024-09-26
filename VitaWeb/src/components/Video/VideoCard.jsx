import ReactPlayer from "react-player/youtube";

export default function VideoCard({ id, title, url, deleteVideo }) {
	return (
		<>
			<div className="my-2 bg-gray-300 p-4 rounded shadow-md mx-auto">
				<div className="flex mb-1">
					<p className="ml-4 md:text-2xl font-bold text-center">
						<span>Titel: </span>
						{title}
					</p>
					<button
						className="px-5 py-1 bg-red-500 ml-auto rounded shadow-md"
						onClick={() => deleteVideo(id)}
					>
						Slet
					</button>
				</div>
				<div className="md:w-[640px] md:h-[360px] relative">
					<ReactPlayer url={url} controls={true} width="100%" height="100%" />
				</div>
			</div>
		</>
	);
}
