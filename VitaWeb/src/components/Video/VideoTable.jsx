import { MdDelete, MdEdit } from "react-icons/md";
import { FaPlay } from "react-icons/fa";

const VideoTable = ({ videos, deleteVideo, handleEdit }) => {
	return (
		<table className="w-full">
			<thead className="sticky top-0">
				<tr className="border-b border-black text-left">
					<th className="p-2">TITEL</th>
					<th className="p-2">BESKRIVELSE</th>
					<th className="p-2 text-center">HANDLINGER</th>
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
						<td className="p-2 flex space-x-8 justify-center items-center">
							<FaPlay
								className="cursor-pointer text-xl hover:text-green-500"
								title="Afspil"
							/>
							<MdEdit
								className="cursor-pointer text-2xl hover:text-slate-500"
								onClick={() => {
									handleEdit(video.title, video.url, video.description);
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
