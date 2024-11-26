import { MdDelete, MdEdit } from "react-icons/md";
import { FaPlay } from "react-icons/fa";
import { useState } from "react";
import VideoModal from "./VideoModal";
import { useVideo } from "../../../hooks/useVideo";

const VideoTable = ({ videos, handleEdit }) => {
	const [showVideoModal, setShowVideoModal] = useState(false);
	const [videoUrl, setVideoUrl] = useState("");
	const { deleteVideo } = useVideo();

	return (
		<table className="w-full">
			{showVideoModal && (
				<VideoModal url={videoUrl} onClose={() => setShowVideoModal(false)} />
			)}
			<thead className="sticky top-0 bg-gradient-to-r from-quaternary via-tertiary to-tertiary text-white">
				<tr className="border-b border-black text-left">
					<th className="p-2">TITEL</th>
					<th className="p-2 hidden sm:flex">BESKRIVELSE</th>
					<th className="p-2 text-right">HANDLINGER</th>
				</tr>
			</thead>
			<tbody>
				{videos.map((video) => (
					<tr
						key={video.id}
						className="hover:bg-gray-100 bg-white bg-opacity-30"
					>
						<td className="p-2 font-semibold">{video.title}</td>
						<td
							dangerouslySetInnerHTML={{ __html: video.description }}
							className="p-2 cursor-default hidden sm:table-cell"
							title={video.description}
						></td>
						<td className="p-2 flex space-x-4 sm:space-x-8 justify-end items-center *:transition-all">
							<FaPlay
								className="cursor-pointer text-xl hover:text-green-500"
								title="Afspil"
								onClick={() => {
									setVideoUrl(video.url);
									setShowVideoModal(true);
								}}
							/>
							<MdEdit
								className="cursor-pointer text-2xl hover:text-slate-500"
								onClick={() => {
									handleEdit({
										id: video.id,
										title: video.title,
										url: video.url,
										description: video.description,
									});
								}}
								title="Rediger"
							/>
							<MdDelete
								className="cursor-pointer text-2xl hover:text-red-700"
								onClick={() => deleteVideo(video.id)}
								title="Slet"
							/>
						</td>
					</tr>
				))}
			</tbody>
		</table>
	);
};

export default VideoTable;
