import React from 'react';
import '../app.css'; 

const VideoCard = ({ video, onDelete }) => {
    return (
        <div className="video-card">
            <iframe 
                width="100%" 
                height="200" 
                src={`https://www.youtube.com/embed/${getVideoIdFromUrl(video.url)}`} 
                title={video.title} 
                allowFullScreen
            ></iframe>
            <h3 className="video-title">{video.title}</h3>
            <p className="video-description">{video.description}</p>
            <button onClick={() => onDelete(video.id)}>Delete</button> {/* Add delete functionality */}
        </div>
    );
};

// Helper function to extract video ID from URL
const getVideoIdFromUrl = (url) => {
    const regex = /(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^&\n]{11})/;
    const match = url.match(regex);
    return match ? match[1] : null;
};

export default VideoCard;
