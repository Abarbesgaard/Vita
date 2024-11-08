import { MdDelete, MdEdit } from "react-icons/md";
import { FaPlay } from "react-icons/fa";
import { useState } from "react";
import VideoModal from "./VideoModal";

const VideoTable = ({ videos, deleteVideo, handleEdit }) => {
	const [showVideoModal, setShowVideoModal] = useState(false);
	const [videoUrl, setVideoUrl] = useState("");

	const onClose = () => {
		setShowVideoModal(false);
	};

	return (
		<table className="w-full">
			{showVideoModal && <VideoModal url={videoUrl} onClose={onClose} />}
			<thead className="sticky top-0 bg-tertiary text-white">
				<tr className="border-b border-black text-left">
					<th className="p-2">TITEL</th>
					<th className="p-2">BESKRIVELSE</th>
					<th className="p-2 text-right">HANDLINGER</th>
				</tr>
			</thead>
			<tbody>
				{videos.map((video) => (
					<tr key={video.id} className="bg-white hover:bg-gray-100">
						<td className="p-2">{video.title}</td>
						<td className="p-2 cursor-default" title={video.description}>
							{video.description.length < 50
								? video.description
								: video.description.substring(0, 50) + "..."}
						</td>
						<td className="p-2 flex space-x-8 justify-end items-center *:transition-all">
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
									handleEdit(
										video.id,
										video.title,
										video.url,
										video.description
									);
								}}
								title="Rediger"
							/>
							<MdDelete
								className="cursor-pointer text-2xl hover:text-red-700"
								onClick={async () => {
									await deleteVideo(video.id);
								}}
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
