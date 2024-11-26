import { createUser } from "../../../services/supabase";
import { useState } from "react";

const UserForm = ({ fetchUsers }) => {
	const [isLoading, setIsLoading] = useState(false);

	const handleSubmit = async (e) => {
		setIsLoading(true);
		e.preventDefault();
		const formData = new FormData(e.target);
		const email = formData.get("email");
		const password = formData.get("password");
		const fullName = formData.get("full_name");
		const userName = formData.get("username");
		await createUser(email, password, fullName, userName);
		await fetchUsers();
		e.target.reset();
		setIsLoading(false);
	};

	return (
		<>
			<h2 className="text-2xl font-bold">Opret ny bruger</h2>
			<form className="w-60 space-y-4 flex flex-col" onSubmit={handleSubmit}>
				<div className="flex flex-col">
					<label>Fulde navn:</label>
					<input
						className="p-1"
						type="text"
						name="full_name"
						placeholder="Indtast navn"
					/>
				</div>
				<div className="flex flex-col">
					<label>Email:</label>
					<input
						className="p-1"
						type="email"
						placeholder="Indtast email"
						name="email"
					/>
				</div>
				<div className="flex flex-col">
					<label>Brugernavn:</label>
					<input
						className="p-1"
						type="text"
						placeholder="Indtast brugernavn"
						name="username"
					/>
				</div>
				<div className="flex flex-col">
					<label>Adgangskode:</label>
					<input
						className="p-1"
						type="password"
						placeholder="Indtast adgangskode"
						name="password"
					/>
				</div>
				<button
					className={`w-20 h-10 bg-tertiary rounded mx-auto ${
						isLoading && "opacity-50 cursor-not-allowed"
					}`}
					type="submit"
				>
					{isLoading ? "Opretter..." : "Opret"}
				</button>
			</form>
		</>
	);
};

export default UserForm;
