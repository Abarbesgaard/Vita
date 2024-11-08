import { AiOutlineVideoCameraAdd } from "react-icons/ai";

const EmptyVideo = ({ handleClick }) => {
	return (
		<>
			<div className="flex flex-col items-center justify-center">
				<AiOutlineVideoCameraAdd className="text-7xl text-slate-600" />
				<p className="font-bold font-mono text-xl">Ingen videoer</p>
				<button className="mt-10" onClick={() => handleClick(true)}>
					<p className="text-white bg-blue-500 hover:bg-blue-600 px-5 py-1 rounded">
						Tilføj ny video
					</p>
				</button>
			</div>
		</>
	);
};

export default EmptyVideo;
