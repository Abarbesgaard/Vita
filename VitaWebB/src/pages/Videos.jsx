import React, { useEffect, useState } from 'react';
import { createVideo, deleteVideo, getAllVideos } from '../APIs/VideoAPI';

export default function Videos() {
    const [videos, setVideos] = useState([]); 
    
    const [newVideo, setNewVideo] = useState({
        title: '',
        description: '',
        url: ''
    });

    // Fetch all videos when the component mounts
    useEffect(() => {
        const fetchVideos = async () => {
            const videoList = await getAllVideos();
            setVideos(videoList);
        };

        fetchVideos();
    }, [videos]);

    // Handle input changes for the new video form
    const handleChange = (e) => {
        const { name, value } = e.target;
        setNewVideo((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    // Handle form submission to add a new video
    const handleSubmit = async (e) => {
        e.preventDefault();
        if (newVideo.title && newVideo.description && newVideo.url) {
            const addedVideo = await createVideo(newVideo); // Get the response from createVideo
            if (addedVideo) { // Check if the video was successfully added
                setVideos((prev) => [...prev, addedVideo]); // Add newly created video to state using API response
                setNewVideo({ title: '', description: '', url: '' }); // Reset form
            } else {
                console.error("Failed to add video."); // Log error if video creation fails
            }
        }
    };
    const handleDelete = async (id) => {
        const result = await deleteVideo(id) // API call 
        if(result) {
            setVideos((prev) => prev.filter(video => video.id !== id))
        }
        else {
            console.log('Kunne ikke slette videoen, prøv igen')
        }
        
    }
    const getVideoIdFromUrl = (url) => {
        const regex = /(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^&\n]{11})/;
        const match = url.match(regex);
        return match ? match[1] : null;
    };

    return (
        <div>
            <h1>Videos</h1>
            <form onSubmit={handleSubmit}>
                <input 
                    type="text" 
                    name="title" 
                    placeholder="Title" 
                    value={newVideo.title} 
                    onChange={handleChange} 
                    required 
                />
                <input 
                    type="text" 
                    name="description" 
                    placeholder="Description" 
                    value={newVideo.description} 
                    onChange={handleChange} 
                    required 
                />
                <input 
                    type="url" 
                    name="url" 
                    placeholder="Video URL" 
                    value={newVideo.url} 
                    onChange={handleChange} 
                    required 
                />
                <button type="submit">Add Video</button>
            </form>

            <h2>Video List</h2>
            <ul className='videoCard'>
                {videos.map(video => (
                    <li key={video.id}>
                        <h3>{video.title}</h3>
                        <p>{video.description}</p>
                        <iframe
                            width="320"
                            height="240"
                            src={`https://www.youtube.com/embed/${getVideoIdFromUrl(video.url)}`}
                            title={video.title}
                            allowFullScreen
                        ></iframe> 
                        <button onClick={() => handleDelete(video.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
}
