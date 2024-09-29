import VideoCard from "./VideoCard";
import { motion } from "framer-motion";
import AccordionItem from "./AccordionItem";
import ReactPlayer from "react-player";
import { useState } from "react";
import { FaYoutube } from "react-icons/fa";

const VideoAccordion = ({ videos, deleteVideo }) => {
	const [isOpen, setIsOpen] = useState(false);

	const toggleAccordion = () => {
		setIsOpen(!isOpen);
	};

	return (
		<>
			{videos.map((video) => {
				return (
					<AccordionItem
						key={video.id}
						title={video.title}
						description={video.description}
						isOpen={isOpen}
						toggleAccordion={toggleAccordion}
					>
						<motion.div
							initial={{ opacity: 0, height: 0 }}
							animate={{ opacity: 1, height: "100%" }}
							exit={{ opacity: 0, height: 0 }}
							transition={{ duration: 0.3, ease: "easeInOut" }}
							className="overflow-hidden flex-shrink-0"
						>
							<div className="flex h-full columns-2 mb-2 justify-center">
								<ReactPlayer
									url={video.url}
									controls={true}
									height="100%"
									light={true}
									playing={true}
									loop={true}
									playIcon={
										<div className="h-full w-full bg-black flex flex-col items-center justify-center">
											<FaYoutube className="hover:scale-110 h-40 w-40 transition-all text-red-600" />
											<p className="text-red-500 font-mono font-bold">
												Afspil video
											</p>
										</div>
									}
								/>
								<textarea className="basis-1/2 bg-gray-300 shadow-inner p-2">
									{video.description}
								</textarea>
							</div>
						</motion.div>
					</AccordionItem>
				);
			})}
		</>
	);
};

export default VideoAccordion;
