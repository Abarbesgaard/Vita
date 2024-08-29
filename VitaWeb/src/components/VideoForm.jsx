/* eslint-disable react/prop-types */
export default function VideoForm({
	handleVideoFormSubmit,
	title,
	setTitle,
	url,
	setUrl,
}) {
	return (
		<>
			<div className="mt-5 bg-white rounded-xl shadow-md">
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
						className="bg-gray-300 pl-2 sm:py-1 shadow-depth_gray rounded sm:w-96"
					/>
					<input
						value={url}
						onChange={(e) => {
							setUrl(e.target.value);
						}}
						type="text"
						placeholder="Indsæt link"
						className="bg-gray-300 pl-2 sm:py-1 shadow-depth_gray rounded sm:w-96"
					/>
					<button
						type="submit"
						className="bg-blue-600 text-white font-semibold rounded shadow-depth_blue active:shadow-none w-24 py-1 hover:bg-blue-700"
					>
						Tilføj
					</button>
				</form>
			</div>
		</>
	);
}
