import { useState, useEffect } from "react";
import { getUsers } from "../../services/supabase";
import UserForm from "./components/UserForm";
import UserList from "./components/UserList";

const StaffPage = () => {
	const [users, setUsers] = useState(null);

	const addUser = () => {
		if (newUserName.trim() !== "") {
			const newUser = {
				id: users.length + 1,
				name: newUserName,
			};
			setUsers([...users, newUser]);
			setNewUserName("");
		}
	};

	const fetchUsers = async () => {
		const { data, error } = await getUsers();
		!error ? setUsers(data) : console.log(error);
	};

	useEffect(() => {
		fetchUsers();
	}, []);

	return (
		<div className="flex space-x-20 h-full pb-5">
			<div>
				<UserList users={users} />
			</div>
			<div className="h-full flex flex-col items-center justify-center">
				<UserForm fetchUsers={fetchUsers} />
			</div>
		</div>
	);
};

export default StaffPage;
