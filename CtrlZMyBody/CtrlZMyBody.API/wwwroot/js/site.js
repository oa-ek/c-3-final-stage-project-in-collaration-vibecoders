(() => {
  const dropdown = document.querySelector('[data-dropdown]');
  const trigger = document.querySelector('[data-dropdown-trigger]');
  const menu = document.querySelector('[data-dropdown-menu]');

  if (dropdown && trigger && menu) {
    trigger.addEventListener('click', (event) => {
      event.stopPropagation();
      dropdown.classList.toggle('open');
    });

    document.addEventListener('click', (event) => {
      if (!dropdown.contains(event.target)) {
        dropdown.classList.remove('open');
      }
    });
  }

  const menuToggle = document.querySelector('[data-menu-toggle]');
  const nav = document.querySelector('[data-nav]');
  if (menuToggle && nav) {
    menuToggle.addEventListener('click', () => {
      nav.classList.toggle('open');
    });
  }

  const timerRoot = document.querySelector('[data-rehab-timer]');
  if (timerRoot) {
    const display = timerRoot.querySelector('[data-timer-display]');
    const minutesInput = timerRoot.querySelector('[data-timer-minutes]');
    const startBtn = timerRoot.querySelector('[data-timer-start]');
    const pauseBtn = timerRoot.querySelector('[data-timer-pause]');
    const resetBtn = timerRoot.querySelector('[data-timer-reset]');

    let remaining = 0;
    let intervalId = null;

    const render = () => {
      const mins = Math.floor(remaining / 60)
        .toString()
        .padStart(2, '0');
      const secs = Math.floor(remaining % 60)
        .toString()
        .padStart(2, '0');
      display.textContent = `${mins}:${secs}`;
    };

    const stop = () => {
      if (intervalId) {
        clearInterval(intervalId);
        intervalId = null;
      }
    };

    startBtn?.addEventListener('click', () => {
      const minutes = Number.parseInt(minutesInput?.value || '0', 10);
      if (!Number.isFinite(minutes) || minutes <= 0) {
        return;
      }

      if (!intervalId) {
        if (remaining <= 0) {
          remaining = minutes * 60;
        }

        intervalId = setInterval(() => {
          if (remaining > 0) {
            remaining -= 1;
            render();
          } else {
            stop();
          }
        }, 1000);
      }

      render();
    });

    pauseBtn?.addEventListener('click', () => {
      stop();
    });

    resetBtn?.addEventListener('click', () => {
      stop();
      remaining = 0;
      render();
    });

    render();
  }
})();

