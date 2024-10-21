export default function VideoForm({
	handleVideoFormSubmit,
	title,
	setTitle,
	url,
	setUrl,
	description,
	setDescription,
}) {
	return (
		<div className="bg-white w-full">
			<form
				className="flex flex-col space-y-4 p-3 items-center"
				onSubmit={handleVideoFormSubmit}
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
					className="bg-gray-50 pl-2 py-1 shadow-depth_gray w-2/3 rounded resize-none"
				/>
				<button
					type="submit"
					className="bg-blue-600 text-white font-semibold rounded shadow-depth_blue active:shadow-none w-24 py-1 hover:bg-blue-700"
				>
					Tilføj
				</button>
			</form>
		</div>
	);
}
