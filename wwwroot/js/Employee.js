// Allows the User to be taken the respective buttons

// How to use scroll-linked animations the right way
// It is Adapted from LogRocket
// https://blog.logrocket.com/use-scroll-linked-animations-right-way/
// Alvin Wan
// https://blog.logrocket.com/author/alvinwan/

document.addEventListener('DOMContentLoaded', () => {
    const scrollLinks = document.querySelectorAll('.scroll-link');

    scrollLinks.forEach(link => {
        link.addEventListener('click', function (event) {
            event.preventDefault();
            const targetId = this.getAttribute('href').substring(1);
            const targetElement = document.getElementById(targetId);

            if (targetElement) {
                window.scrollTo({
                    top: targetElement.offsetTop,
                    behavior: 'smooth'
                });
            }
        });
    });
});

// Toggle Dark Mode
const darkModeToggle = document.querySelector('.dark-mode');
const body = document.querySelector('body');

darkModeToggle.addEventListener('click', () => {
    body.classList.toggle('dark-mode-variables');
    darkModeToggle.querySelectorAll('span').forEach((el) => {
        el.classList.toggle('active');
    });
});

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.card').forEach(card => {
        card.addEventListener('click', function () {
            const farmerId = this.getAttribute('data-farmer-id');
            console.log(`Fetching products for farmer ID: ${farmerId}`);
            fetchFarmerProducts(farmerId);
        });
    });
});


function fetchFarmerProducts(farmerId) {
    fetch(`/Employee/GetFarmerProducts?farmerId=${farmerId}`)
        .then(response => response.json())
        .then(data => {
            console.log('Farmer products data:', data);
            const dashboardContent = document.getElementById('dashboard-content');
            dashboardContent.innerHTML = `
                <button id="back-btn"><span class="material-icons-sharp">arrow_back</span></button>
                <h1>${data.name}'s Products</h1>
                ${data.productsByCategory.map(category => `
                    <h2>${category.category}</h2>
                    <div class="grid">
                        ${category.products.map(product => `
                            <div class="card">
                                <div class="card-header">
                                    <img src="${product.imageUri}" alt="${product.name}" class="profile-img">
                                </div>
                                <div class="card-body">
                                    <h3>${product.name}</h3>
                                    <p>Production Date: ${product.productionDate}</p>
                                </div>
                            </div>
                        `).join('')}
                    </div>
                `).join('')}
            `;

            document.getElementById('back-btn').addEventListener('click', () => {
                console.log('Reloading page to go back');
                location.reload(); // Simple way to go back to the original view
            });
        })
        .catch(error => console.error('Error fetching farmer products:', error));
}



fetch('/Employee/GetVideos')
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
function filterProducts() {
    const category = document.querySelector('.search-category input').value;
    const startDate = document.getElementById('start-date').value;
    const stopDate = document.getElementById('stop-date').value;

    console.log(`Category: ${category}, Start Date: ${startDate}, Stop Date: ${stopDate}`);

    fetch('/Employee/FilterProductsByDate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            category: category,
            startDate: startDate,
            stopDate: stopDate,
        }),
    })
        .then(response => {
            console.log('Response received from the server');
            return response.json();
        })
        .then(data => {
            console.log('Data received from the server:', data);

            const productResults = document.getElementById('product-results');
            productResults.innerHTML = '';

            if (data.length === 0) {
                console.log('No products found');
                productResults.innerHTML = '<p>No products found</p>';
            } else {
                data.forEach(product => {
                    const productCard = document.createElement('div');
                    productCard.classList.add('card');

                    // Log the image URI to debug
                    console.log(`Loading image from: ${product.imageUri}`);

                    productCard.innerHTML = `
                    <br>
                    <div class="card-header">
                        <img src="${product.imageUri}" alt="${product.name}" class="profile-img" onerror="this.src='/images/default.png'">
                    </div>
                    <div class="card-body">
                        <h3>${product.name}</h3>
                        <p>Category: ${product.category}</p>
                        <p>Production Date: ${product.productionDate}</p>
                        <p>Farmer: ${product.farmerName}</p>
                    </div>
                `;
                    productResults.appendChild(productCard);
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

// Ensure the filterProducts function is available when the DOM is fully loaded
document.addEventListener('DOMContentLoaded', () => {
    document.querySelector('.search-btn').addEventListener('click', filterProducts);
});
function showFarmerProducts(farmerId) {
    fetch(`/Employee/GetFarmerProducts?farmerId=${farmerId}`)
        .then(response => response.json())
        .then(data => {
            const dashboardContent = document.getElementById('dashboard-content');
            dashboardContent.innerHTML = `
                                <button id="back-btn"><span class="material-icons-sharp">arrow_back</span></button>
                                <h1>${data.name}'s Products</h1>
                                ${data.productsByCategory.map(category => `
                                    <h2>${category.category}</h2>
                                    <div class="grid">
                                        ${category.products.map(product => `
                                            <div class="card">
                                                <div class="card-header">
                                                    <img src="${product.imageUri}" alt="${product.name}" class="profile-img">
                                                </div>
                                                <div class="card-body">
                                                    <h3>${product.name}</h3>
                                                    <p>Production Date: ${product.productionDate}</p>
                                                </div>
                                            </div>
                                        `).join('')}
                                    </div>
                                `).join('')}
                            `;

            document.getElementById('back-btn').addEventListener('click', () => {
                location.reload(); // Simple way to go back to the original view
            });
        })
        .catch(error => console.error('Error fetching farmer products:', error));
}
function showAdditionalDetails(farmerId, cardElement) {
    console.log(`Fetching additional details for farmer ID: ${farmerId}`);
    fetch(`/Employee/GetFarmerDetails?farmerId=${farmerId}`)
        .then(response => response.json())
        .then(data => {
            console.log('Farmer details:', data);
            const detailsHtml = `
                <h3>${data.name}</h3>
                <p>Farm Size: ${data.farmSize}</p>
                <p>Type of Farming: ${data.typeOfFarming}</p>
                <p>Years of Experience: ${data.yearsOfExperience}</p>
                <p>Certifications: ${data.certifications}</p>
            `;
            const modal = document.getElementById('farmer-details-modal');
            document.getElementById('farmer-details').innerHTML = detailsHtml;

            // Calculate position
            const rect = cardElement.getBoundingClientRect();
            modal.style.top = `${rect.top + window.scrollY}px`;
            modal.style.left = `${rect.right + 10}px`;

            modal.style.display = 'block';
        })
        .catch(error => console.error('Error fetching farmer details:', error));
}


function hideAdditionalDetails() {
    document.getElementById('farmer-details-modal').style.display = 'none';
}

function hideFarmerDetails() {
    document.getElementById('dashboard-content').style.display = 'block';
    document.getElementById('farmer-details').style.display = 'none';
}

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.info-icon').forEach(icon => {
        icon.addEventListener('click', function (event) {
            event.stopPropagation();
            const farmerId = this.closest('.card').getAttribute('data-farmer-id');
            console.log(`Fetching additional details for farmer ID: ${farmerId}`);
            showAdditionalDetails(farmerId, this.closest('.card'));
        });
    });
});

// Modal close button
const modalCloseBtn = document.querySelector('#farmer-details-modal .close');
modalCloseBtn.addEventListener('click', () => {
    hideAdditionalDetails();
});

// Close modal on outside click
window.addEventListener('click', (event) => {
    const modal = document.getElementById('farmer-details-modal');
    if (event.target !== modal && !modal.contains(event.target)) {
        hideAdditionalDetails();
    }
});
