/* eslint-disable react/prop-types */
export default function UserCard({ user }) {
	return (
		<div className="bg-white rounded-xl shadow-md flex flex-col lg:mb-20 mt-10 lg:mt-0">
			<div className="bg-blue-300 w-full h-28 rounded-t-xl"></div>
			<div className="-mt-28 p-2 pr-20">
				<img
					src={user.picture}
					width={150}
					className="rounded-full border-white border-8 ml-2 mt-2"
				/>
			</div>
			<div className="flex flex-col items-end p-10 pt-1">
				<p className="text-2xl md:text-3xl">{user.name}</p>
				<p className="text-xl md:text-2xl">{user.email}</p>
			</div>
		</div>
	);
}
