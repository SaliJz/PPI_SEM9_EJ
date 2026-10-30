using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private float tickInterval = 1f; // segundos reales por tick
    [SerializeField] private int ticksPerDay = 10;

    public int Day { get; private set; } = 1;
    public event Action<int> OnDayPassed;
    // Observer: lista de suscriptores que quieren enterarse de cada tick
    private readonly List<ITickable> tickables = new List<ITickable>();
    private float timer;

    public int TickCount { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Register(ITickable tickable)
    {
        if (tickable != null && !tickables.Contains(tickable))
        {
            tickables.Add(tickable);
        }
    }

    public void Unregister(ITickable tickable) => tickables.Remove(tickable);

    private void Update()
    {
        timer += Time.deltaTime;
        while (timer >= tickInterval)
        {
            timer -= tickInterval;
            Tick();
        }
    }

    private void Tick()
    {
        TickCount++;
        // ToArray: copia para poder registrar/quitar suscriptores durante el tick
        foreach (var t in tickables.ToArray()) t.OnTick();

        if (TickCount % ticksPerDay == 0)
        {
            Day++;
            OnDayPassed?.Invoke(Day);
        }
    }
}