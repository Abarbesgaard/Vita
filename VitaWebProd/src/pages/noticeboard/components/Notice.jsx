import { FaUserEdit } from "react-icons/fa";
import moment from "moment";

import { useAuth } from "../../../hooks/useAuth";

const Notice = ({ notice }) => {
	const { user } = useAuth();
	console.log(notice);

	return (
		<div className="p-3 shadow border relative bg-white">
			<p className="font-bold text-lg px-2">{notice.title}</p>
			<hr />
			<textarea
				className="w-full resize-none bg-transparent border-none mt-3 pb-10"
				disabled
				value={notice.content}
			/>
			<div className="flex items-center absolute bottom-2">
				<FaUserEdit className="opacity-75" />
				<p className="ml-1 text-xs opacity-50">{user.user_metadata.fullName}</p>
			</div>
			<div className="flex items-center absolute bottom-2 right-2">
				<p className="ml-5 mr-1 text-xs opacity-50">
					{moment(notice.createdAt).format("DD/MM/YY - HH:mm")}
				</p>
			</div>
		</div>
	);
};

export default Notice;
