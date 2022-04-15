#!/usr/bin/env node

import inquirer from "inquirer";
import puppeteer from "puppeteer";
import { createSpinner } from "nanospinner";

async function promptUrl() {
  const answers = await inquirer.prompt({
    type: "input",
    name: "url",
    message: "Url to comb?",
    validate(value) {
      try {
        new URL(value);
        return true;
      } catch {
        return "Please enter a valid url";
      }
    },
    default: "https://news.ycombinator.com",
  });

  return answers.url;
}

async function comb(url) {
  const spinner = createSpinner("Combing...").start();

  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(url, {
    waitUntil: "networkidle2",
  });

  spinner.update({
    text: "Generating pdf",
  });

  await page.pdf({ path: "hn.pdf", format: "a4" });

  await browser.close();

  spinner.update({
    text: "Pdf file created ./hn.pdf",
  });

  spinner.success();
}

const url = await promptUrl();
await comb(url);

//console.clear();
//process.exit(1);
