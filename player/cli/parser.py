import argparse

class CLIParser:
    def __init__(self) -> None:
        self.__parser = argparse.ArgumentParser(description="Wordle CLI", formatter_class=argparse.ArgumentDefaultsHelpFormatter)
        self.__parser.add_argument("-c", "--cli", action="store_true", help="Run Wordle using CLI")
        self.__parser.add_argument("-g", "--gui", action="store_true", help="Run Wordle using GUI")
        
    def parse(self):
        return self.__parser.parse_args()
        