document.addEventListener("DOMContentLoaded", function () {
    const menuBtn = document.getElementById("menu-btn");
    const closeBtn = document.getElementById("close-btn");
    const sidebar = document.querySelector("aside .sidebar");
    const darkModeToggle = document.querySelectorAll(".dark-mode span");
    const root = document.documentElement;

    fetch('/Farmer/GetVideos')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(videos => {
            console.log('Videos received:', videos);  // Log the received videos array
            const videosContainer = document.getElementById('videos-container');
            videos.forEach(video => {
                console.log('Video URL:', video.url);  // Log the URL
                console.log('Thumbnail Path:', video.thumbnail);  // Log the thumbnail path

                const videoCard = document.createElement('div');
                videoCard.className = 'video-card';

                videoCard.innerHTML = `
            <a href="${video.url}" target="_blank">
                <img src="${video.thumbnail}" alt="${video.title}">
            </a>
            <div class="video-info">
                <h3>${video.title}</h3>
            </div>
        `;
                videosContainer.appendChild(videoCard);
            });
        })
        .catch(error => console.error('Error fetching videos:', error));

    menuBtn.addEventListener("click", () => {
        sidebar.classList.add("active");
    });

    closeBtn.addEventListener("click", () => {
        sidebar.classList.remove("active");
    });

    darkModeToggle.forEach((btn) => {
        btn.addEventListener("click", () => {
            root.classList.toggle("dark-mode-variables");
        });
    });
});
