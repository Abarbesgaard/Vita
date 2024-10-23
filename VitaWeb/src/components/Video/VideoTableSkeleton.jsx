import { MdDelete, MdEdit } from "react-icons/md";
import { FaPlay } from "react-icons/fa";

const videoSkeletons = Array.from({ length: 8 }, (_, i) => ({
	id: i,
	title: "Loading...",
	description: "Loading...",
}));

const VideoTableSkeleton = () => {
	return (
		<table className="w-full">
			<thead className="sticky top-0">
				<tr className="border-b border-black text-left">
					<th className="p-2">TITEL</th>
					<th className="p-2">BESKRIVELSE</th>
					<th className="p-2 text-right">HANDLINGER</th>
				</tr>
			</thead>
			<tbody>
				{videoSkeletons.map((video) => (
					<tr key={video.id} className="bg-white">
						<td className="p-2">
							<div
								className={`h-2 w-20 rounded-md bg-gray-500 animate-pulse`}
							></div>
						</td>
						<td className="p-2">
							<div
								className={`h-2 w-full rounded-md bg-gray-500 animate-pulse`}
							></div>
						</td>
						<td className="p-2 flex space-x-8 justify-end items-center">
							<FaPlay className="cursor-pointer text-xl text-gray-500 animate-pulse" />
							<MdEdit className="cursor-pointer text-2xl text-gray-500 animate-pulse" />
							<MdDelete className="cursor-pointer text-2xl text-gray-500 animate-pulse" />
						</td>
					</tr>
				))}
			</tbody>
		</table>
	);
};

export default VideoTableSkeleton;
