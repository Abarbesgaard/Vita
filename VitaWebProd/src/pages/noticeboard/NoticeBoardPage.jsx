import { useEffect, useState } from "react";
import NoticeList from "./components/NoticeList";
import { IoIosAddCircleOutline } from "react-icons/io";
import moment from "moment";
import { makeServer } from "../../APIs/mockAPI";

// makeServer();

moment.updateLocale("da-DK", {
	culture: "da-DK",
});

const NoticeBoard = () => {
	const [notices, setNotices] = useState(null);

	const handleAddNotice = async () => {
		const response = await fetch("api/notices", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
		});
		const data = await response.json();
		console.log(data);
		setNotices([...notices, data.notice]);
	};

	useEffect(() => {
		const getNotices = async () => {
			const response = await fetch("api/notices");
			const data = await response.json();
			setNotices(data.notices);
			console.log(data);
		};
		getNotices();
	}, []);

	if (!notices) {
		return (
			<div className="w-full h-screen flex items-center justify-center">
				<h1>Loading...</h1>
			</div>
		);
	}

	return (
		<div className="w-full h-full overflow-auto">
			<div className="flex flex-col items-center">
				<div className="w-full grid grid-cols-2 grid-flow-row gap-3 mb-5 justify-center">
					<NoticeList notices={notices} />
				</div>
				<IoIosAddCircleOutline
					onClick={handleAddNotice}
					className="text-4xl cursor-pointer active:scale-100 hover:rotate-90 hover:scale-125 transition-all"
				/>
			</div>
		</div>
	);
};

export default NoticeBoard;
