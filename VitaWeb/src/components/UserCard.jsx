export default function UserCard({ user }) {
	return (
		<div className="bg-white rounded-xl shadow-md flex flex-col lg:mb-20 mb-2 mt-2 lg:mt-0">
			<div className="bg-blue-300 w-full h-16 md:h-28 rounded-t-xl"></div>
			<div className="flex md:flex-col -mt-12 md:-mt-28 p-2 md:items-center">
				<div className="w-20 md:w-[150px]">
					<img
						src={user.picture}
						className="rounded-full border-white border-4 md:border-8 ml-2 md:ml-0 md:mt-2 -mt-2"
					/>
				</div>
				<div className="flex flex-col items-end md:items-center p-4 pt-1">
					<p className="text-2xl md:text-3xl">{user.name}</p>
					<p className="text-xl md:text-2xl">{user.email}</p>
				</div>
			</div>
		</div>
	);
}
