import { MdDelete } from "react-icons/md";
import { MdEdit } from "react-icons/md";

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
								className={`h-2 w-10 rounded-md bg-gray-500 animate-pulse`}
							></div>
						</td>
						<td className="p-2">
							<div className="w-full">
								<div
									className={`h-2 w-${
										Math.floor(Math.random() * (5 - 1 + 1)) + 1
									}/6 rounded-md bg-gray-500 animate-pulse`}
								></div>
							</div>
						</td>
						<td className="p-2 flex space-x-8 justify-end">
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
