import { useMemo } from "react";
import Notice from "./Notice";
import moment from "moment";

const NoticeList = ({ notices }) => {
	const sortedNotices = useMemo(() => {
		return [...(notices || [])].sort(
			(a, b) => moment(b.createdAt) - moment(a.createdAt)
		);
	}, [notices]);

	return (
		<>
			{sortedNotices &&
				sortedNotices.map((notice) => (
					<Notice key={notice.id} notice={notice} />
				))}
		</>
	);
};

export default NoticeList;
