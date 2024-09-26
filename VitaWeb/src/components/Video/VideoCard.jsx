import ReactPlayer from "react-player/youtube";

export default function VideoCard({ video, deleteVideo }) {
	return (
		<>
			<div className="my-2 bg-gray-300 p-4 rounded shadow-md mx-auto w-3/4">
				<div className="flex mb-1">
					<p className="ml-4 md:text-2xl font-bold text-center">
						{video.title}
					</p>
					<button
						className="px-5 py-1 bg-red-500 ml-auto rounded shadow-md"
						onClick={() => deleteVideo(video.id)}
					>
						Slet
					</button>
				</div>
				<div className="md:w-[640px] md:h-[360px] relative mx-auto">
					<ReactPlayer
						url={video.url}
						controls={true}
						width="100%"
						height="100%"
					/>
				</div>
				<textarea
					className="text-balance w-full mt-3 resize-none px-2 shadow-inner"
					disabled
					rows={5}
					value={video.description}
				/>
			</div>
		</>
	);
}
