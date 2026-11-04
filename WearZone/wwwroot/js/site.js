// CART DRAWER LOGIC

document.addEventListener('DOMContentLoaded', function () {
  refreshCart(); // Check cart on load
});

/* SEARCH OVERLAY */
function openSearch() {
  const overlay = document.getElementById('searchOverlay');
  overlay.classList.add('open');
  setTimeout(() => {
    overlay.querySelector('input').focus();
  }, 100);
}

function closeSearch() {
  document.getElementById('searchOverlay').classList.remove('open');
}

function openCart() {
  document.getElementById('cartDrawer').classList.add('open');
  document.getElementById('cartOverlay').classList.add('open');
  refreshCart();
}

function closeCart() {
  document.getElementById('cartDrawer').classList.remove('open');
  document.getElementById('cartOverlay').classList.remove('open');
}

/* WISHLIST FUNCTIONALITY */
function toggleWishlist(productId, btnElement) {
  fetch('/api/wishlist/toggle', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(productId)
  })
    .then(response => {
      if (response.status === 401) {
        window.location.href = '/Identity/Account/Login'; // Redirect if not logged in
        return;
      }
      return response.json();
    })
    .then(data => {
      if (data) {
        if (data.added) {
          btnElement.classList.add('active');
        } else {
          btnElement.classList.remove('active');
          if (window.location.pathname.includes('/Wishlist')) {
            location.reload();
          }
        }
      }
    })
    .catch(err => console.error('Error toggling wishlist:', err));
}

function refreshCart() {
  fetch('/api/cart')
    .then(response => response.json())
    .then(data => updateCartUI(data))
    .catch(err => console.error('Error loading cart:', err));
}

function addToCart(productId, btnElement) {
  // Visual feedback
  const originalText = btnElement ? btnElement.innerText : null;
  if (btnElement) {
    btnElement.innerText = "ADDING...";
    btnElement.disabled = true;
  }

  fetch('/api/cart/add', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(productId)
  })
    .then(response => response.json())
    .then(data => {
      updateCartUI(data);

      if (btnElement) {
        btnElement.innerText = "ADDED";
        setTimeout(() => {
          btnElement.innerText = originalText;
          btnElement.disabled = false;
        }, 2000);
      }

      openCart(); // Auto open drawer
    })
    .catch(err => {
      console.error('Error adding to cart:', err);
      if (btnElement) {
        btnElement.innerText = "ERROR";
        setTimeout(() => {
          btnElement.innerText = originalText;
          btnElement.disabled = false;
        }, 2000);
      }
    });
}

function removeFromCart(productId) {
  fetch('/api/cart/remove', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(productId)
  })
    .then(response => response.json())
    .then(data => updateCartUI(data))
    .catch(err => console.error('Error removing from cart:', err));
}

function updateCartUI(data) {
  // data = { items: [], total: 0, count: 0 }

  // Update Badges
  const badge = document.getElementById('cartBadge');
  const headerCount = document.getElementById('cartCountHeader');

  if (data.count > 0) {
    badge.innerText = data.count;
    badge.style.display = 'flex';
    headerCount.innerText = data.count;
  } else {
    badge.style.display = 'none';
    headerCount.innerText = '0';
  }

  // Update Total
  document.getElementById('cartTotal').innerText = '$' + data.total.toFixed(2);

  // Render Items
  const container = document.getElementById('cartItemsContainer');
  container.innerHTML = '';

  if (data.items.length === 0) {
    container.innerHTML = '<p style="padding:20px; color:#999; text-align:center;">Your bag is empty.</p>';
    return;
  }

  data.items.forEach(item => {
    const div = document.createElement('div');
    div.className = 'cart-item';
    div.innerHTML = `
            <img src="${item.imageUrl}" alt="${item.productName}" />
            <div class="cart-item-details">
                <div class="cart-item-header">
                    <div class="cart-item-title">${item.productName}</div>
                    <span class="cart-item-remove" onclick="removeFromCart(${item.productId})">&times;</span>
                </div>
                <div class="cart-item-price">$${item.price.toFixed(2)}</div>
                
                <div class="cart-qty-control">
                    <button class="btn-qty" onclick="decreaseQuantity(${item.productId})">-</button>
                    <span class="qty-display">${item.quantity}</span>
                    <button class="btn-qty" onclick="addToCart(${item.productId})">+</button>
                </div>
            </div>
        `;
    container.appendChild(div);
  });
}

function decreaseQuantity(productId) {
  fetch('/api/cart/decrease', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(productId)
  })
    .then(response => response.json())
    .then(data => updateCartUI(data))
    .catch(err => console.error('Error decreasing cart item:', err));
}
