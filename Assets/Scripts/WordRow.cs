using System;
using System.Collections.Generic;
using UnityEngine;

public class WordRow : MonoBehaviour {

    public int currentSize = 0;
    public WordGridButton gridButtonPrefab;
    public List<WordGridButton> wordgridButtons;

    bool shaking = false;
    public float shakeTime = 1f;
    float stopshaketime = 0;
    public float direction = 1;
    public float shakeAmount = 50f;
    public float shakeSpeed = 200f;
    public float currentTarget = 0;
    public void Shake() {
        shaking = true;
        stopshaketime = Time.time + shakeTime;
    }

    private void Update() {
        if (shaking) {
            if (Time.time > stopshaketime) {
                MoveTowardsZeroX();
                if (transform.localPosition.x == 0) {
                    shaking = false;
                }
            } else {
                if (transform.localPosition.x != currentTarget) {
                    MoveTowardsTarget();
                } else {
                    direction *= -1;
                    currentTarget = direction * shakeAmount;
                    MoveTowardsTarget();
                }
            }
        } 
    }

    public void MoveTowardsZeroX() {
        transform.localPosition = new Vector3(Mathf.MoveTowards(transform.localPosition.x, 0 , shakeSpeed * Time.deltaTime), transform.localPosition.y, transform.localPosition.z);
    }

    public void MoveTowardsTarget() {
        transform.localPosition = new Vector3(Mathf.MoveTowards(transform.localPosition.x, currentTarget, shakeSpeed * Time.deltaTime), transform.localPosition.y, transform.localPosition.z);
    }


    public void SetSize(int size) {
        currentSize = size;
        int toadd = size - wordgridButtons.Count ;
        for (int i = 0; i < toadd; i++) {
            WordGridButton button = Instantiate(gridButtonPrefab, transform);
            wordgridButtons.Add(button);
        }

        foreach (WordGridButton g in wordgridButtons) {
            g.gameObject.SetActive(false);
        }

        for (int i = 0; i < size; i++) {
            wordgridButtons[i].gameObject.SetActive(true);
            wordgridButtons[i].SetEmpty();
        }
    }

    public string storedString;


    public void SetTypeText(string s) {
        storedString = s;
        for (int i = 0; i < currentSize; i++) {
            if (i < s.Length) {
                wordgridButtons[i].SetTypedLetter(s[i]);
            } else {
                wordgridButtons[i].SetEmpty();
            }
        }
    }
}
