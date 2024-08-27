/* eslint-disable react/prop-types */
export default function VideoCard({ id, title, url }) {
	return (
		<div className="border-b border-black w-9/12 h-[1000px]">
			<p className="mt-4 mb-1 ml-4 text-2xl font-bold">
				<span>{id}.</span>
				{title}
			</p>
			<iframe
				className="h-[200px] sm:h-[375px] md:h-[500px] w-full"
				src={url}
				title="YouTube video player"
				referrerPolicy="strict-origin-when-cross-origin"
				allowFullScreen
			/>
		</div>
	);
}
