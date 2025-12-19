const SIZE = 5;
const STORAGE_KEY = 'bingo-state';

const table = document.getElementById('bingo');
const resetBtn = document.getElementById('reset');

let items = [];
let state = [];

// ---------- INIT ----------
fetch('items.txt')
    .then(r => r.text())
    .then(text => {
        items = text
            .split('\n')
            .map(x => x.trim())
            .filter(Boolean);

        loadOrCreate();
        render();
    });

// ---------- STATE ----------
function loadOrCreate() {
    const saved = localStorage.getItem(STORAGE_KEY);
    if (saved) {
        const data = JSON.parse(saved);
        items = data.items;
        state = data.state;
    } else {
        shuffle(items);
        items = items.slice(0, SIZE * SIZE);
        state = Array.from({ length: SIZE * SIZE }, () => false);
        save();
    }
}

function save() {
    localStorage.setItem(STORAGE_KEY, JSON.stringify({ items, state }));
}

// ---------- RENDER ----------
function render() {
    table.innerHTML = '';
    let k = 0;

    for (let i = 0; i < SIZE; i++) {
        const tr = document.createElement('tr');

        for (let j = 0; j < SIZE; j++) {
            const td = document.createElement('td');
            td.textContent = items[k];
            if (state[k]) td.classList.add('selected');

            const index = k;
            td.onclick = () => {
                state[index] = !state[index];
                save();
                td.classList.toggle('selected');
            };

            tr.appendChild(td);
            k++;
        }
        table.appendChild(tr);
    }
}

// ---------- RESET ----------
resetBtn.onclick = () => {
    localStorage.removeItem(STORAGE_KEY);
    location.reload();
};

// ---------- UTILS ----------
function shuffle(arr) {
    for (let i = arr.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [arr[i], arr[j]] = [arr[j], arr[i]];
    }
}
