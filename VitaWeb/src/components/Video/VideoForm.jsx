import { useState } from "react";
import { PiSpinnerGap } from "react-icons/pi";

export default function VideoForm({
	handleVideoFormSubmit,
	title,
	setTitle,
	url,
	setUrl,
	description,
	setDescription,
	mode,
}) {
	const [isLoading, setIsLoading] = useState(false);

	if (isLoading) {
		return (
			<div className="flex items-center justify-center w-full h-full">
				<PiSpinnerGap className="animate-spin text-5xl" />
			</div>
		);
	}

	return (
		<div className="bg-white w-full">
			<form
				className="flex flex-col space-y-4"
				onSubmit={() => {
					setIsLoading(true);
					handleVideoFormSubmit();
				}}
			>
				<input
					value={title}
					onChange={(e) => {
						setTitle(e.target.value);
					}}
					type="text"
					placeholder="Indsæt titel"
					className="bg-gray-50 pl-2 py-1 shadow-depth_gray w-2/3 rounded"
				/>
				<input
					value={url}
					onChange={(e) => {
						setUrl(e.target.value);
					}}
					type="text"
					placeholder="Indsæt link"
					className="bg-gray-50 pl-2 py-1 shadow-depth_gray w-2/3 rounded"
				/>
				<textarea
					value={description}
					onChange={(e) => {
						setDescription(e.target.value);
					}}
					rows={5}
					placeholder="Indsæt beskrivelse"
					className="bg-gray-50 pl-2 py-1 shadow-depth_gray w-full rounded resize-none"
				/>
				<button
					type="submit"
					className="bg-blue-600 text-white font-semibold rounded shadow-depth_blue active:shadow-none w-24 py-1 hover:bg-blue-700"
				>
					{mode ? "Opdater" : "Tilføj"}
				</button>
			</form>
		</div>
	);
}
