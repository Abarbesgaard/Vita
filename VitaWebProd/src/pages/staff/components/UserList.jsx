const UserList = ({ users }) => {
	return (
		<div className="h-full">
			<p className="font-bold">Brugere:</p>
			{users && (
				<ul className="p-5 overflow-auto grid grid-cols-4 gap-2">
					{users.map((user) => (
						<li
							key={user.id}
							className="flex flex-col shadow bg-primary bg-opacity-50 rounded-lg w-60"
						>
							<div className="p-5">
								<p className="font-semibold font-lato">{user.full_name}</p>
								<p className="opacity-50 font-lato">{user.email}</p>
							</div>
							<div className="flex w-full mt-auto">
								<div className="flex flex-1 items-center justify-center border-t-2 border-r">
									<button className="w-full p-2 font-semibold font-mono hover:bg-secondary hover:bg-opacity-40">
										Rediger
									</button>
								</div>
								<div className="flex flex-1 items-center justify-center border-t-2 border-l">
									<button className="w-full p-2 font-semibold font-mono hover:bg-secondary hover:bg-opacity-40">
										Slet
									</button>
								</div>
							</div>
						</li>
					))}
				</ul>
			)}
		</div>
	);
};

export default UserList;
